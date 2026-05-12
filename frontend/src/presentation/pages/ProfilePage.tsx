import {Fragment, memo} from "react";
import {WebsiteLayout} from "@presentation/layouts/WebsiteLayout";
import {UserAddForm} from "@presentation/components/forms/User/UserAddForm.tsx";


export const ProfilePage = memo(() => {
    return <Fragment>
        <WebsiteLayout>
            <div>
                <UserAddForm></UserAddForm>
            </div>
        </WebsiteLayout>
    </Fragment>
});