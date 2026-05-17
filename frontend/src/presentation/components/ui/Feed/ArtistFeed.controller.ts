import { useGetArtworksByArtistProfileId } from "@infrastructure/apis/api-management/artwork.ts";
import { useGetArtistProfileByUserId } from "@infrastructure/apis/api-management/artistProfile.ts";
import { usePaginationController } from "@presentation/components/ui/Tables/Pagination.controller.ts";
import { ArtworkOrderEnum, ArtworkRecord } from "@infrastructure/apis/client";
import { useAppSelector } from "@application/store.ts";
import { useEffect, useState } from "react";

export const useArtistFeedController = () => {
    const { loggedIn, userId } = useAppSelector(x => x.profileReducer);
    const { page, pageSize, setPage } = usePaginationController();

    const [searchInput, setSearchInput] = useState("");
    const [searchQuery, setSearchQuery] = useState("");

    const { data: artistProfile, isFetching: isArtistProfileFetching } = useGetArtistProfileByUserId(userId);
    const artistProfileId = artistProfile?.response?.id ?? "";

    const { data, isFetching: isArtistArtworksFetching } = useGetArtworksByArtistProfileId(
        artistProfileId,
        page,
        pageSize,
        searchQuery,
        loggedIn && !!artistProfileId,
    );

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
    }, [searchQuery, artistProfileId, setPage]);

    useEffect(() => {
        const incomingArtworks: ArtworkRecord[] = data?.response?.data ?? [];

        if (page === 1) {
            setArtworksList(incomingArtworks);
        } else {
            setArtworksList((prev) => {
                const existingIds = new Set(prev.map((item) => item.id));
                const uniqueNewItems = incomingArtworks.filter((item) => !existingIds.has(item.id));
                return [...prev, ...uniqueNewItems];
            });
        }
    }, [data, page]);

    const hasMore = (data?.response?.data ?? []).length === pageSize;
    const loading = isArtistArtworksFetching || isArtistProfileFetching;

    const loadMore = () => {
        if (!loading && hasMore) {
            setPage(page + 1);
        }
    };

    console.log(artistProfileId);
    console.log(artworksList);

    return {
        artworks: artworksList,
        loading,
        hasMore,
        loadMore,
        searchInput,
        setSearchInput,
        handleSearchSubmit,
        handleSearchClear,
        artistProfile,
        artistProfileId,
    };
};
