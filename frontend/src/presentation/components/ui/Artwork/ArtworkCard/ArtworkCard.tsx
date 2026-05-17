import { ArtworkRecord } from "@infrastructure/apis/client";
import { useArtworkCardController } from "./ArtworkCard.controller";
import IconButton from '@mui/material/IconButton';
import ThumbUpOutlinedIcon from '@mui/icons-material/ThumbUpOutlined';
import ShareOutlinedIcon from '@mui/icons-material/ShareOutlined';
import VisibilityOutlinedIcon from '@mui/icons-material/VisibilityOutlined';

export const ArtworkCard = ({ artwork }: { artwork: ArtworkRecord }) => {
    const { imageUrl, likes, shares, views, handleLike, handleShare, isActionLoading } = useArtworkCardController(artwork);

    return (
        <div className="museum-panel p-4 border rounded-lg shadow-md flex flex-col h-full">
            {imageUrl ? (
                <img
                    src={imageUrl}
                    alt={artwork.title}
                    className="gallery-frame object-cover rounded-md mb-4"
                />
            ) : (
                <div className="flex items-center justify-center">
                    Loading image...
                </div>
            )}

            <div className="flex-1">
                <h3 className="text-lg font-semibold mb-2">{artwork.title}</h3>
                <p className="text-gray-600 mb-4 line-clamp-3">{artwork.description}</p>
            </div>

            <div className="flex items-center justify-between text-sm text-gray-500 mb-3">
                <div className="flex items-center gap-3">
                    <div className="flex items-center gap-1">
                        <VisibilityOutlinedIcon fontSize="small" />
                        <span>{views}</span>
                    </div>
                    <div className="flex items-center gap-1">
                        <ThumbUpOutlinedIcon fontSize="small" />
                        <span>{likes}</span>
                    </div>
                    <div className="flex items-center gap-1">
                        <ShareOutlinedIcon fontSize="small" />
                        <span>{shares}</span>
                    </div>
                </div>
            </div>

            <div className="flex items-center justify-between">
                <IconButton
                    size="small"
                    color="primary"
                    onClick={handleLike}
                    disabled={isActionLoading}
                    className="border border-gray-300"
                >
                    <ThumbUpOutlinedIcon />
                </IconButton>
                <IconButton
                    size="small"
                    color="primary"
                    onClick={handleShare}
                    disabled={isActionLoading}
                    className="border border-gray-300"
                >
                    <ShareOutlinedIcon />
                </IconButton>
            </div>
        </div>
    );
};