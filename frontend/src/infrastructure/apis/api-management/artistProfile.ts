import { useAppSelector } from "@application/store";
import { Configuration, ArtistProfileApi } from "@infrastructure/apis/client";
import { useQuery } from "@tanstack/react-query";

const getArtistProfileQueryKey = "getArtistProfileQuery";

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
