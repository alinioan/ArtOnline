import { Button, Dialog, DialogContent, DialogTitle } from "@mui/material";
import { useUserAddDialogController } from "./ArtworkAddDialog.controller.ts";
import { UserAddForm } from "@presentation/components/forms/User/UserAddForm";
import { useIntl } from "react-intl";
import {ArtworkAddForm} from "@presentation/components/forms/Artwork/ArtworkAddForm.tsx";

/**
 * This component wraps the user add form into a modal dialog.
 */
export const ArtworkAddDialog = () => {
  const { open, close, isOpen } = useUserAddDialogController();
  const { formatMessage } = useIntl();

  return <div>
    <Button className="nav-icon-button" variant="outlined" onClick={open}>
      Create
    </Button>
    <Dialog
      open={isOpen}
      onClose={close}>
      <DialogTitle>
        Add Artwork
      </DialogTitle>
      <DialogContent>
        <ArtworkAddForm onSubmit={close} />
      </DialogContent>
    </Dialog>
  </div>
};