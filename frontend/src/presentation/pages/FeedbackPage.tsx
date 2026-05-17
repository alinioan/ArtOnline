import {Fragment, memo} from "react";
import {Seo} from "@presentation/components/ui/Seo";
import {WebsiteLayout} from "@presentation/layouts/WebsiteLayout";
import {Box} from "@mui/material";
import {LoginForm} from "@presentation/components/forms/Login/LoginForm.tsx";
import {FeedbackForm} from "@presentation/components/forms/Feedback/FeedbackForm.tsx";

export const FeedbackPage = memo(() => {
    return <Fragment>
        <WebsiteLayout>
            <div className="bg-white p-10">
                <FeedbackForm></FeedbackForm>
            </div>
        </WebsiteLayout>
    </Fragment>
});