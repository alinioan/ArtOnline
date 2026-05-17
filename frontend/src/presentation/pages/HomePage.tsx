import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Seo } from "@presentation/components/ui/Seo";
import { useAppSelector } from "@application/store.ts";
import { useAppRouter } from "@infrastructure/hooks/useAppRouter";
import { AppRoute } from "../../routes.ts";
import { Feed } from "@presentation/components/ui/Feed/Feed.tsx";
import { useFeedController } from "@presentation/components/ui/Feed/Feed.controller.ts";
import Button from "@mui/material/Button";
import {ArtworkOrderEnum} from "@infrastructure/apis/client";

export const HomePage = memo(() => {
  const { loggedIn } = useAppSelector(x => x.profileReducer);
  const { navigate } = useAppRouter();
    const {
        artworks,
        loading,
        hasMore,
        loadMore,
        searchInput,
        setSearchInput,
        handleSearchSubmit,
        handleSearchClear,
        sort,
        setSort
    } = useFeedController();

    const onFormSubmit = (e: React.FormEvent) => {
        e.preventDefault(); // Prevents the browser from reloading the page
        handleSearchSubmit();
    };

  return <Fragment>
      <Seo title="MobyLab Web App | Home" />
      <WebsiteLayout>
        <div>
          {!loggedIn && (
            <div className="flex items-center justify-center min-h-[40vh]">
              <div className="museum-panel p-12 w-full max-w-md">
                <h1 className="text-center mb-8 text-5xl">Welcome!</h1>
                <div className="flex flex-col gap-10">
                  <button
                    onClick={() => navigate(AppRoute.Login)}
                    className="auth-button-primary"
                  >
                    Log In
                  </button>
                  <button
                    onClick={() => navigate(AppRoute.Register)}
                    className="auth-button-secondary"
                  >
                    Register
                  </button>
                </div>
              </div>
            </div>
          )}
            {loggedIn && (
                <div>
                    <Feed
                        artworks={artworks}
                        loading={loading}
                        hasMore={hasMore}
                        onLoadMore={loadMore}
                        searchInput={searchInput}
                        setSearchInput={setSearchInput}
                        onSearchSubmit={handleSearchSubmit}
                        sort={sort}
                        setSort={setSort}
                    />
                </div>
            )}
        </div>
      </WebsiteLayout>
    </Fragment>
});
