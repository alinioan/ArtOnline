import { Button, CircularProgress, FormControl, FormHelperText, FormLabel, OutlinedInput, Stack } from "@mui/material";
import { FormattedMessage, useIntl } from "react-intl";
import { useArtistProfileUpdateFormController } from "./ArtistProfileUpdateForm.controller.ts";
import { isEmpty, isUndefined } from "lodash";

export const ArtistProfileUpdateForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useArtistProfileUpdateFormController(props.onSubmit);

    return <form onSubmit={actions.handleSubmit(actions.submit)}>
        <Stack spacing={4} style={{ width: "100%" }}>
            <div className="grid grid-cols-1 gap-y-5 gap-x-5">
                <div className="col-span-1" style={{ display: "none" }}>
                    <FormControl fullWidth>
                        <OutlinedInput
                            {...actions.register("id")}
                            type="hidden"
                        />
                    </FormControl>
                </div>
                <div className="col-span-1">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.bio)}
                    >
                        <FormLabel>
                            Bio
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("bio")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: "Bio",
                                })}
                            autoComplete="none"
                            multiline
                            rows={5}
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.bio)}
                        >
                            {state.errors.bio?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <Button className="-col-end-1 col-span-1" type="submit" disabled={!isEmpty(state.errors) || computed.isSubmitting}>
                    {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                    {computed.isSubmitting && <CircularProgress />}
                </Button>
            </div>
        </Stack>
    </form>;
};
