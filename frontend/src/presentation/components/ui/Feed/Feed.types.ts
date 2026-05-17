import {ArtworkOrderEnum, ArtworkRecord} from "@infrastructure/apis/client";

export interface Props {
    artworks: ArtworkRecord[];
    loading: boolean;
    hasMore: boolean;
    onLoadMore: () => void;
    searchInput: string;
    setSearchInput: (value: string) => void;
    onSearchSubmit: () => void;
    sort: ArtworkOrderEnum;
    setSort: (value: ArtworkOrderEnum) => void;
}