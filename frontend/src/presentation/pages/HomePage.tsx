import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Typography } from "@mui/material";
import { Fragment, memo } from "react";
import { useIntl } from "react-intl";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";

export const HomePage = memo(() => {
  const { formatMessage } = useIntl();

  return <Fragment>
      <Seo title="MobyLab Web App | Home" />
      <WebsiteLayout>
        <div className="pl-[50px] pr-[50px]">

        </div>
      </WebsiteLayout>
    </Fragment>
});
