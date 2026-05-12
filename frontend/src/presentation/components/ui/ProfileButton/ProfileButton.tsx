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

export default function ProfileButton() {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const dispatch = useAppDispatch();
    const {redirectToHome} = useAppRouter();
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
            <IconButton
                id="fade-button"
                aria-controls={open ? 'fade-menu' : undefined}
                aria-haspopup="true"
                aria-expanded={open ? 'true' : undefined}
                onClick={handleClick}
                // className="bg-white text-black"
            >
                <AccountCircle />
            </IconButton>
            <Menu
                id="fade-menu"
                slotProps={{
                    list: {
                        'aria-labelledby': 'fade-button',
                    },
                }}
                slots={{ transition: Fade }}
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
            >
                <MenuItem href={AppRoute.Profile}> <Link className={"text-black"} to={AppRoute.Profile}>Profile</Link> </MenuItem>
                <MenuItem onClick={logout}>Logout</MenuItem>
            </Menu>
        </div>
    );
}
