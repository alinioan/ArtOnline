import {CircularProgress, FormControl, FormHelperText, FormLabel, OutlinedInput, Stack} from "@mui/material";
import {FormattedMessage, useIntl} from "react-intl";
import {UploadButton} from "@presentation/components/ui/UploadButton";
import {isEmpty, isUndefined} from "lodash";
import Button from "@mui/material/Button";
import {useArtworkAddFormController} from "@presentation/components/forms/Artwork/ArtworkAddForm.controller.ts";

export const ArtworkAddForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useArtworkAddFormController(props.onSubmit);

    return <form onSubmit={actions.handleSubmit(actions.submit)}>
        <Stack spacing={4} style={{ width: "100%" }}>
            <div className="grid grid-cols-2 gap-y-5 gap-x-5">
                <div className="col-span-1">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.title)}
                    >
                        <FormLabel>
                            Title
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("title")}
                            placeholder={formatMessage(
                                { id: "Please enter Title" },
                                {
                                    fieldName: "",
                                })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.title)}
                        >
                            {state.errors.description?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <div className="col-span-1">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.description)}
                    >
                        <FormLabel>
                            <FormattedMessage id="globals.description" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("description")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: formatMessage({
                                        id: "globals.description",
                                    }),
                                })}
                            autoComplete="none"
                        /> {/* Add a input like a textbox shown here. */}
                        <FormHelperText
                            hidden={isUndefined(state.errors.description)}
                        >
                            {state.errors.description?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <div className="col-span-1 flex justify-center items-center">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.file)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.file" />
                        </FormLabel>
                        <UploadButton
                            onUpload={actions.setFile}
                            isLoading={computed.isSubmitting}
                            disabled={computed.isSubmitting}
                            text={formatMessage({ id: "labels.addUserFile" })}
                            acceptFileType="*/*" />
                        <FormHelperText
                            hidden={isUndefined(state.errors.file)}
                        >
                            {state.errors.file?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <Button className="-col-end-1 col-span-1" type="submit" disabled={!isEmpty(state.errors) || computed.isSubmitting}>
                    {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                    {computed.isSubmitting && <CircularProgress />}
                </Button>
            </div>
        </Stack>
    </form>
};