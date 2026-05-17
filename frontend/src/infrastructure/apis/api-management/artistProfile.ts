import { useAppSelector } from "@application/store";
import { Configuration, ArtistProfileApi, ArtistProfileAddRecord } from "@infrastructure/apis/client";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";

const getArtistProfileQueryKey = "getArtistProfileQuery";
const addArtistProfileMutationKey = "addArtistProfileMutation";

const getArtistProfileApiFactory = (token: string | null) => new ArtistProfileApi(new Configuration({ accessToken: token ?? "" }));

export const useGetArtistProfileByUserId = (userId: string | null) => {
    const { token } = useAppSelector(x => x.profileReducer);

    return {
        ...useQuery({
            queryKey: [getArtistProfileQueryKey, token, userId],
            queryFn: async () => await getArtistProfileApiFactory(token).apiArtistProfileGetByUserIdUserIdGet({ userId: userId ?? "" }),
            refetchInterval: Infinity,
            refetchOnWindowFocus: false,
            enabled: !!userId
        }),
        queryKey: getArtistProfileQueryKey
    };
};

export const useAddArtistProfile = () => {
    const { token } = useAppSelector(x => x.profileReducer);
    const queryClient = useQueryClient();

    return useMutation({
        mutationKey: [addArtistProfileMutationKey, token],
        mutationFn: async (artistProfileAddRecord: ArtistProfileAddRecord) => {
            const result = await getArtistProfileApiFactory(token).apiArtistProfileAddPost({ artistProfileAddRecord });
            await queryClient.invalidateQueries({ queryKey: [getArtistProfileQueryKey], type: "all" });

            return result;
        }
    });
};
