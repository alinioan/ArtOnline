import {Props} from "@presentation/components/ui/Feed/Feed.types.ts";
import {ArtworkCard} from "@presentation/components/ui/Artwork/ArtworkCard";
import {useEffect, useRef} from "react";
import {ArtworkOrderEnum} from "@infrastructure/apis/client";

export const Feed = ({
     artworks,
     loading,
     hasMore,
     onLoadMore,
     searchInput,
     setSearchInput,
     onSearchSubmit,
     sort,
     setSort
}: Props) => {
    const observerTarget = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        const target = observerTarget.current;
        if (!target) return;

        const observer = new IntersectionObserver(
            (entries) => {
                // Trigger fetch if sentinel is visible, data is available, and we aren't currently loading
                if (entries[0].isIntersecting && hasMore && !loading) {
                    onLoadMore();
                }
            },
            { threshold: 0.1 }
        );

        observer.observe(target);

        return () => {
            if (target) observer.unobserve(target);
        };
    }, [loading, hasMore, onLoadMore]);

    return (
        <div className="feed-panel space-y-6">

            {/* Flex Control Header Container */}
            <div className="flex flex-wrap gap-2 justify-between items-center">
                <form
                    onSubmit={(e) => { e.preventDefault(); onSearchSubmit(); }}
                    className="flex gap-2 flex-1 max-w-sm"
                >
                    <input
                        type="text"
                        value={searchInput}
                        onChange={(e) => setSearchInput(e.target.value)}
                        placeholder="Search artworks..."
                        className="border rounded p-1.5 text-sm flex-1 outline-none focus:ring-1 focus:ring-blue-500"
                    />
                    <button
                        type="submit"
                        className="rounded px-3 text-sm transition-colors"
                    >
                        Search
                    </button>
                </form>

                <select
                    value={sort}
                    onChange={(e) => setSort(e.target.value as ArtworkOrderEnum)}
                    className="nav-icon-button border rounded p-1.5"
                >
                    <option className="nav-link" value="Newest">New</option>
                    <option className="nav-link" value="Oldest">Old</option>
                    <option className="nav-link" value="MostLiked">Likes</option>
                    <option className="nav-link" value="MostViewed">Views</option>
                </select>
            </div>

            {/* Grid Area */}
            <div className="feed-scroll-area grid grid-cols-1 md:grid-cols-3 gap-5">
                {artworks.map((art) => (
                    <ArtworkCard key={art.id} artwork={art} />
                ))}
            </div>

            {/* Invisible Tripwire + Loading States */}
            <div ref={observerTarget} className="h-10 mt-8 flex items-center justify-center text-gray-500 text-sm">
                {loading && <p>Loading more masterpieces...</p>}
                {!hasMore && artworks.length > 0 && <p>You've viewed all available artwork.</p>}
            </div>

        </div>
    );
};