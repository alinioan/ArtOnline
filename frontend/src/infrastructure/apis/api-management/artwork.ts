import { Configuration, ArtworkApi, ArtworkLikeApi } from "@infrastructure/apis/client";
import { useAppSelector } from "@application/store.ts";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useMemo } from "react";

const getArtworkImageQueryKey = "getArtworkImageQuery";
const getArtworkApiQueryKey = "getArtworkApiQuery";

const getArtworkApiFactory = (token: string | null) => new ArtworkApi(new Configuration({ accessToken: token ?? "" }));
const getArtworkLikeApiFactory = (token: string | null) => new ArtworkLikeApi(new Configuration({ accessToken: token ?? "" }));

export const useGetArtworkImage = (id: string) => {
    const { token } = useAppSelector(x => x.profileReducer);
    return {
        ...useQuery({
            queryKey: [getArtworkImageQueryKey, token, id],
            queryFn: async () => await getArtworkApiFactory(token).apiArtworkGetImageIdGet({ id }),
            refetchInterval: Infinity,
            refetchOnWindowFocus: false,
            enabled: !!id,
        }),
        queryKey: getArtworkImageQueryKey,
    };
};

export const useIncrementArtworkViews = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const queryClient = useQueryClient();
    const api = useMemo(() => getArtworkApiFactory(token), [token]);

    return useMutation({
        mutationFn: async (id: string) => await api.apiArtworkIncrementViewsIdPut({ id }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getFeedQuery"] });
        },
    });
};

export const useIncrementArtworkShares = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const queryClient = useQueryClient();
    const api = useMemo(() => getArtworkApiFactory(token), [token]);

    return useMutation({
        mutationFn: async (id: string) => await api.apiArtworkIncrementSharesIdPut({ id }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getFeedQuery"] });
        },
    });
};

export const useLikeArtwork = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const queryClient = useQueryClient();
    const api = useMemo(() => getArtworkLikeApiFactory(token), [token]);

    return useMutation({
        mutationFn: async (artworkId: string) => await api.apiArtworkLikeLikePost({ body: artworkId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getFeedQuery"] });
        },
    });
};

export const useAddArtwork = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const queryClient = useQueryClient();
    const api = useMemo(() => getArtworkApiFactory(token), [token]);

    return useMutation({
        mutationFn: async (payload: { title: string; description?: string; imageFile: File; artistProfileId?: string }) => {
            return await api.apiArtworkAddPost({
                title: payload.title,
                description: payload.description,
                imageFile: payload.imageFile,
                artistProfileId: payload.artistProfileId,
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getFeedQuery"] });
        },
    });
};