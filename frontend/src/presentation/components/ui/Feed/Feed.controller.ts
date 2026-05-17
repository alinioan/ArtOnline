import { useGetFeed } from "@infrastructure/apis/api-management/feed.ts";
import { usePaginationController } from "@presentation/components/ui/Tables/Pagination.controller.ts";
import {ArtworkOrderEnum, ArtworkRecord} from "@infrastructure/apis/client";
import { useAppSelector } from "@application/store.ts";
import {useEffect, useState} from "react";

export const useFeedController = () => {
    const { loggedIn } = useAppSelector(x => x.profileReducer);
    const { page, pageSize, setPage, setPageSize } = usePaginationController();

    const [searchInput, setSearchInput] = useState("");
    const [searchQuery, setSearchQuery] = useState("");
    const [sort, setSort] = useState<ArtworkOrderEnum>("Newest");

    const { data, isFetching } = useGetFeed(page, pageSize, searchQuery, sort, loggedIn);
    const [artworksList, setArtworksList] = useState<ArtworkRecord[]>([]);

    const handleSearchSubmit = () => {
        setSearchQuery(searchInput);
    };

    const handleSearchClear = () => {
        setSearchInput("");
        setSearchQuery("");
    };

    useEffect(() => {
        setPage(1);
    }, [searchQuery, sort, setPage]);

    useEffect(() => {
        const incomingArtworks = data?.result?.data ?? [];

        if (page === 0 || page === 1) {
            setArtworksList(incomingArtworks);
        } else {
            setArtworksList((prev) => {
                const existingIds = new Set(prev.map(item => item.id));
                const uniqueNewItems = incomingArtworks.filter(item => !existingIds.has(item.id));
                return [...prev, ...uniqueNewItems];
            });
        }
    }, [data, page]);

    const hasMore = (data?.result?.data ?? []).length === pageSize;

    const loadMore = () => {
        if (!isFetching && hasMore) {
            setPage(page + 1);
        }
    };

    return {
        artworks: artworksList,
        loading: isFetching,
        hasMore,
        loadMore,
        searchInput,
        setSearchInput,
        handleSearchSubmit,
        handleSearchClear,
        sort,
        setSort,
    };
};