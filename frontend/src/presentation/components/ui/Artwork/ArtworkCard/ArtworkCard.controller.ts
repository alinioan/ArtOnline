import {
    useDeleteArtwork,
    useGetArtworkImage,
    useIncrementArtworkShares,
    useIncrementArtworkViews,
    useLikeArtwork
} from "@infrastructure/apis/api-management";
import { ArtworkRecord } from "@infrastructure/apis/client";
import { useMemo, useEffect, useState, useRef } from "react";

export const useArtworkCardController = (artwork: ArtworkRecord) => {
    const { data: imageBlob } = useGetArtworkImage(artwork.id);
    const likeMutation = useLikeArtwork();
    const shareMutation = useIncrementArtworkShares();
    const viewMutation = useIncrementArtworkViews();
    const deleteMutation = useDeleteArtwork();

    const [likes, setLikes] = useState(artwork.likes);
    const [shares, setShares] = useState(artwork.shares);
    const [views, setViews] = useState(artwork.views);
    const hasTrackedInitialView = useRef(false);

    const imageUrl = useMemo(() => imageBlob ? URL.createObjectURL(imageBlob) : undefined, [imageBlob]);

    useEffect(() => {
        return () => {
            if (imageUrl) {
                URL.revokeObjectURL(imageUrl);
            }
        };
    }, [imageUrl]);

    const handleLike = () => {
        likeMutation.mutate(artwork.id, {
            onSuccess: () => {
                setLikes((current) => current + 1);
            },
        });
    };

    const handleShare = () => {
        shareMutation.mutate(artwork.id, {
            onSuccess: () => {
                setShares((current) => current + 1);
            },
        });
    };

    const handleDelete = () => {
        deleteMutation.mutate(artwork.id, {
            onSuccess: () => {
                window.location.reload();
            },
        })
    }

    useEffect(() => {
        if (!artwork.id || hasTrackedInitialView.current) {
            return;
        }

        hasTrackedInitialView.current = true;

        viewMutation.mutate(artwork.id, {
            onSuccess: () => {
                setViews((current) => current + 1);
            },
        });
    }, [artwork.id, viewMutation]);

    return {
        imageUrl,
        artwork,
        likes,
        shares,
        views,
        handleLike,
        handleShare,
        handleDelete,
    };
};