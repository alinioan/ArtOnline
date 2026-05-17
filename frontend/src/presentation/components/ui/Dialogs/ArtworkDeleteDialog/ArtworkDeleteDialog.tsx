import {
    useUserAddDialogController
} from "@presentation/components/ui/Dialogs/ArtworkAddDialog/ArtworkAddDialog.controller.ts";
import {useIntl} from "react-intl";
import {Button, Dialog, DialogContent, DialogTitle, IconButton} from "@mui/material";
import {ArtworkAddForm} from "@presentation/components/forms/Artwork/ArtworkAddForm.tsx";
import {
    useArtworkDeleteDialogController
} from "@presentation/components/ui/Dialogs/ArtworkDeleteDialog/ArtworkDeleteDialog.controller.ts";
import {DeleteForever} from "@mui/icons-material";

export const ArtworkDeleteDialog = ({handleDelete}) => {
    const { open, close, isOpen } = useArtworkDeleteDialogController();
    const { formatMessage } = useIntl();

    return <div>
        <IconButton
            size="small"
            color="primary"
            onClick={open}
            className="border border-gray-300"
        >
            <DeleteForever></DeleteForever>
        </IconButton>
        <Dialog
            open={isOpen}
            onClose={close}>
            <DialogTitle>
                Delete Artwork
            </DialogTitle>
            <DialogContent>
                <Button className="card" onClick={close}>Nevermind :)</Button>
                <Button className="card" onClick={handleDelete}>DELETE {">"}:( </Button>
            </DialogContent>
        </Dialog>
    </div>
}