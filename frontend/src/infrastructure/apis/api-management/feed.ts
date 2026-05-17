import { ArtworkOrderEnum, Configuration, FeedApi } from "@infrastructure/apis/client";
import { useAppSelector } from "@application/store.ts";
import { useQuery } from "@tanstack/react-query";

const getFeedQueryKey = "getFeedQuery";

const getFactory = (token: string | null) => new FeedApi(new Configuration({ accessToken: token ?? "" }));

export const useGetFeed = (
    page: number,
    pageSize: number,
    search: string = "",
    sort: ArtworkOrderEnum = "Newest",
    enabled: boolean = true,
) => {
    const { token } = useAppSelector(x => x.profileReducer);

    return useQuery({
        queryKey: [getFeedQueryKey, token, page, pageSize, search, sort],
        queryFn: async () => {
            const response = await getFactory(token).apiFeedGetFeedGetRaw({ page, pageSize, search, sort });
            return await response.raw.json();
        },
        enabled,
        refetchInterval: Infinity,
        refetchOnWindowFocus: false,
    });
};
