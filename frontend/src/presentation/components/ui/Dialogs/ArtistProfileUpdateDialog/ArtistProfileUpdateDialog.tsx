import { Dialog, DialogContent, DialogTitle, IconButton } from "@mui/material";
import { useArtistProfileUpdateDialogController } from "./ArtistProfileUpdateDialog.controller.ts";
import { ArtistProfileUpdateForm } from "@presentation/components/forms/ArtistProfile/ArtistProfileUpdateForm.tsx";
import { EditRounded } from "@mui/icons-material";

/**
 * This component wraps the user add form into a modal dialog.
 */
export const ArtistProfileUpdateDialog = () => {
  const { open, close, isOpen } = useArtistProfileUpdateDialogController();

  return <div>
    <IconButton onClick={open} size="small" className="nav-icon-button w-8 h-8 p-1">
      <EditRounded fontSize="small" />
    </IconButton>
    <Dialog
      open={isOpen}
      onClose={close}>
      <DialogTitle>
        Update Bio
      </DialogTitle>
      <DialogContent>
        <ArtistProfileUpdateForm onSubmit={close} />
      </DialogContent>
    </Dialog>
  </div>
};