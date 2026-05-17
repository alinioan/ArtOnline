import {Fragment, memo} from "react";
import {WebsiteLayout} from "@presentation/layouts/WebsiteLayout";
import {UserAddForm} from "@presentation/components/forms/User/add/UserAddForm.tsx";
import {UserUpdateForm} from "@presentation/components/forms/User/updated/UserUpdateForm.tsx";
import Button from "@mui/material/Button";
import { useOwnUser } from "@infrastructure/hooks/useOwnUser";
import { useAddArtistProfile } from "@infrastructure/apis/api-management/artistProfile";
import {toast} from "react-toastify";


export const ProfilePage = memo(() => {
    const ownUser = useOwnUser();
    const { mutate: addArtistProfile, isPending } = useAddArtistProfile();

    const handleBecomeArtist = () => {
        if (ownUser?.id) {
            addArtistProfile({
                userId: ownUser.id,
                bio: ""
            });
            toast("Added artist profile");
        }
    };

    return <Fragment>
        <WebsiteLayout>
            <div className="flex2">
                <div className="bg-white">
                    <UserUpdateForm></UserUpdateForm>
                </div>
                <Button 
                    className="nav-icon-button"
                    onClick={handleBecomeArtist}
                    disabled={isPending}
                >
                    {isPending ? "Loading..." : "Become an artist."}
                </Button>
            </div>
        </WebsiteLayout>
    </Fragment>
});