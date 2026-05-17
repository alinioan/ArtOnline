import {Fragment, memo, useEffect, useRef} from "react";
import { WebsiteLayout } from "@presentation/layouts/WebsiteLayout";
import { Feed } from "@presentation/components/ui/Feed/Feed.tsx";
import { useArtistFeedController } from "@presentation/components/ui/Feed/ArtistFeed.controller.ts";
import { useAppSelector } from "@application/store.ts";
import {ArtworkCard} from "@presentation/components/ui/Artwork/ArtworkCard";
import {IconButton} from "@mui/material";
import {EditRounded} from "@mui/icons-material";
import {ArtistProfileUpdateDialog} from "@presentation/components/ui/Dialogs/ArtistProfileUpdateDialog";

export const ArtistPage = memo(() => {
    const { loggedIn } = useAppSelector(x => x.profileReducer);
    const {
        artworks,
        loading,
        hasMore,
        loadMore,
        searchInput,
        setSearchInput,
        handleSearchSubmit,
        handleSearchClear,
        artistProfile,
    } = useArtistFeedController();

    const observerTarget = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        const target = observerTarget.current;
        if (!target) return;

        const observer = new IntersectionObserver(
            (entries) => {
                // Trigger fetch if sentinel is visible, data is available, and we aren't currently loading
                if (entries[0].isIntersecting && hasMore && !loading) {
                    loadMore();
                }
            },
            { threshold: 0.1 }
        );

        observer.observe(target);

        return () => {
            if (target) observer.unobserve(target);
        };
    }, [loading, hasMore, loadMore]);

    return <Fragment>
        <WebsiteLayout>
            {!loggedIn ? (
                <div className="flex items-center justify-center min-h-[40vh]">
                    <div className="museum-panel p-10 text-center">
                        <h2 className="text-2xl font-semibold mb-4">Please log in</h2>
                        <p>Log in to view artworks from your artist profile.</p>
                    </div>
                </div>
            ) : (
                <div className="flex flex-col lg:flex-row gap-6">
                    <div className="feed-panel lg:w-2/3">
                        <div className="feed-scroll-area grid grid-cols-1 md:grid-cols-3 gap-5">
                            {artworks.map((art) => (
                                <ArtworkCard key={art.id} artwork={art} hasDelete={true} />
                            ))}
                        </div>

                        <div ref={observerTarget} className="h-10 mt-8 flex items-center justify-center text-gray-500 text-sm">
                            {loading && <p>Loading more masterpieces...</p>}
                            {!hasMore && artworks.length > 0 && <p>You've viewed all available artwork.</p>}
                        </div>
                    </div>

                    <aside className="lg:w-1/3 museum-panel p-6">
                        <div className="flex gap-10 items-start justify-between">
                            <div className="flex2">
                                <h3 className="text-xl mb-3">Artist profile</h3>
                                {artistProfile?.response ? (
                                    <div className="space-y-3 text-sm text-gray-800">
                                            <p><strong>Bio:</strong> {artistProfile.response.bio ?? "No bio available."}</p>
                                        <p><strong>Artwork count:</strong> {artistProfile.response.artworkIds?.length ?? 0}</p>
                                    </div>
                                ) : (
                                    <p className="text-gray-500">Fetching your artist profile data...</p>
                                )}
                            </div>
                            <ArtistProfileUpdateDialog></ArtistProfileUpdateDialog>
                        </div>
                    </aside>
                </div>
            )}
        </WebsiteLayout>
    </Fragment>
});