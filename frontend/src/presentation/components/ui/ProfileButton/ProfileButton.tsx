import * as React from 'react';
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Fade from '@mui/material/Fade';
import {Link} from "react-router-dom";
import {AppRoute} from "../../../../routes.ts";
import {useCallback} from "react";
import {resetProfile} from "@application/state-slices";
import {useAppDispatch} from "@application/store.ts";
import {useAppRouter} from "@infrastructure/hooks/useAppRouter.ts";
import IconButton from '@mui/material/IconButton';
import AccountCircle from '@mui/icons-material/AccountCircle';
import {string} from "yup";
import {useOwnUserHasRole} from "@infrastructure/hooks/useOwnUser.ts";
import {UserRoleEnum} from "@infrastructure/apis/client";

interface ProfileButtonProps {
    className?: string
}

export default function ProfileButton({className}: ProfileButtonProps) {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const dispatch = useAppDispatch();
    const {redirectToHome} = useAppRouter();
    const isArtist = useOwnUserHasRole(UserRoleEnum.Artist);
    const logout = useCallback(() => {
        dispatch(resetProfile());
        redirectToHome();
    }, [dispatch, redirectToHome]);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <div>
            <IconButton className={className}
                id="fade-button"
                aria-controls={open ? 'fade-menu' : undefined}
                aria-haspopup="true"
                aria-expanded={open ? 'true' : undefined}
                onClick={handleClick}
            >
                <AccountCircle/>
            </IconButton>
            <Menu
                id="fade-menu"
                slotProps={{
                    list: {
                        'aria-labelledby': 'fade-button',
                    },
                    paper: {
                        sx: {
                            background: "linear-gradient(to bottom, #241116, #16090d)",
                            color: "var(--accent-ivory)",
                            border: "1px solid rgba(198, 169, 114, 0.15)",
                            backdropFilter: "blur(14px)",
                            boxShadow: "0 10px 35px rgba(0, 0, 0, 0.28)",
                        }
                    }
                }}
                slots={{transition: Fade}}
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
            >
                <MenuItem href={AppRoute.Profile} sx={{padding: 0}} onClick={handleClose}> 
                    <Link to={AppRoute.Profile} className="nav-link px-4 py-2 w-full block">
                        Profile
                    </Link>
                </MenuItem>
                {isArtist &&
                    <MenuItem href={AppRoute.ArtistProfile} sx={{padding: 0}} onClick={handleClose}>
                        <Link to={AppRoute.ArtistProfile} className="nav-link px-4 py-2 w-full block">
                            Artist Profile
                        </Link>
                    </MenuItem>}
                <MenuItem sx={{padding: 0}}>
                    <Link to={AppRoute.Feedback} className="nav-link px-4 py-2 w-full block">
                        Feedback
                    </Link>
                </MenuItem>
                <MenuItem onClick={logout} sx={{padding: 0}}>
                    <div className="nav-link px-4 py-2 w-full">Logout</div>
                </MenuItem>
            </Menu>
        </div>
    );
}
