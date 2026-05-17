import {
    Button,
    CircularProgress,
    FormControl,
    FormHelperText,
    FormLabel,
    Stack,
    OutlinedInput
} from "@mui/material";
import { FormattedMessage, useIntl } from "react-intl";
import { useUserUpdateFormController } from "./UserUpdateForm.controller.ts";
import { isEmpty, isUndefined } from "lodash";
import { UserUpdateFormModel } from "./UserUpdateForm.types.ts";

/**
 * Here we declare the user update form component.
 * This form may be used in modals so the onSubmit callback could close the modal on completion.
 */
export const UserUpdateForm = (props: { initialData?: UserUpdateFormModel; onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useUserUpdateFormController(props.initialData, props.onSubmit); // Use the controller.

    const handleFormSubmit = actions.handleSubmit((data) => {
        console.log("Form validation passed, submitting data:", data);
        actions.submit(data);
    }, (errors) => {
        console.log("Form validation failed, errors:", errors);
    });

    return <form onSubmit={handleFormSubmit}> {/* Wrap your form into a form tag and use the handle submit callback to validate the form and call the data submission. */}
        <Stack spacing={4} style={{ width: "100%" }}>
            <div className="grid grid-cols-2 gap-y-5 gap-x-5">
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
                        error={!isUndefined(state.errors.name)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.name" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("name")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: formatMessage({
                                        id: "globals.name",
                                    }),
                                })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.name)}
                        >
                            {state.errors.name?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <div className="col-span-1">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.password)}
                    >
                        <FormLabel required>
                            New Password
                        </FormLabel>
                        <OutlinedInput
                            type="password"
                            {...actions.register("password")}
                            placeholder={"New Password"}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.password)}
                        >
                            {state.errors.password?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <Button className="-col-end-1 col-span-1" type="submit" disabled={computed.isSubmitting}> {/* Add a button with type submit to call the submission callback if the button is a descended of the form element. */}
                    {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                    {computed.isSubmitting && <CircularProgress />}
                </Button>
            </div>
        </Stack>
    </form>
};
