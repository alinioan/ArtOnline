import {
    Button,
    CircularProgress,
    FormControl,
    FormHelperText,
    FormLabel,
    Stack,
    OutlinedInput,
    Select,
    MenuItem,
    RadioGroup,
    FormControlLabel,
    Radio,
    Checkbox
} from "@mui/material";
import { FormattedMessage, useIntl } from "react-intl";
import { useFeedbackFormController } from "./FeedbackForm.controller";
import { isEmpty, isUndefined } from "lodash";

export const FeedbackForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useFeedbackFormController(props.onSubmit);

    return <form onSubmit={actions.handleSubmit(actions.submit)}>
        <Stack spacing={4} style={{ width: "100%" }}>
            <div className="grid grid-cols-2 gap-y-5 gap-x-5" color="inherit">
                <div className="col-span-2">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.category)}
                    >
                        <FormLabel required>
                            <h2 className="text-xl">Aspects of the gallery that needs improvements</h2>
                        </FormLabel>
                        <Select
                            {...actions.register("category")}
                            value={actions.watch("category")}
                            onChange={actions.selectCategory}
                            displayEmpty
                        >
                            <MenuItem value="" disabled>
                                <span className="text-gray">
                                    {formatMessage({ id: "globals.placeholders.selectInput" }, {
                                            fieldName: "aspect of the gallery you want to comment on",
                                    })}
                                </span>
                            </MenuItem>
                            <MenuItem value="Artwork collection">
                                Artwork Collection
                            </MenuItem>
                            <MenuItem value="ux">
                                User Experience
                            </MenuItem>
                            <MenuItem value="perf&acc">
                                Performance & Accessibility
                            </MenuItem>
                        </Select>
                        <FormHelperText
                            hidden={isUndefined(state.errors.category)}
                        >
                            {state.errors.category?.message}
                        </FormHelperText> 
                    </FormControl>
                </div>
                <div className="col-span-2" color="inherit">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.rating)}
                        color="inherit"
                    >
                        <FormLabel required>
                            <h2 className="text-xl">Is this feature to your liking?</h2>
                        </FormLabel>
                            <RadioGroup
                                value={actions.watch("rating")}
                                onChange={actions.selectRating}
                                row
                            >
                            <FormControlLabel
                                value="good"
                                control={<Radio />}
                                label="YES!"
                            />
                            <FormControlLabel
                                value="neutral"
                                control={<Radio />}
                                label="ehhhhhh..."
                            />
                            <FormControlLabel
                                value="bad"
                                control={<Radio />}
                                label="No, this sucks so much!!!!"
                            />
                        </RadioGroup>
                        <FormHelperText
                            hidden={isUndefined(state.errors.rating)}
                        >
                            {state.errors.rating?.message}
                        </FormHelperText>
                    </FormControl>
                </div>
                <div className="col-span-2">
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.message)}
                    >
                        <FormLabel required>
                            <h2 className="text-xl">Tell us more</h2>
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("message")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: "your comment",
                                })}
                            multiline
                            rows={4}
                            autoComplete="none"
                        /> 
                        <FormHelperText
                            hidden={isUndefined(state.errors.message)}
                        >
                            {state.errors.message?.message}
                        </FormHelperText> 
                    </FormControl>
                </div>
                <div className="col-span-2">
                    <FormControlLabel
                        control={<Checkbox
                            {...actions.register("subscribe")}
                            checked={actions.watch("subscribe")}
                        />}
                        label="Share your email for contact with us"
                    />
                </div>
                <Button className="-col-end-1 col-span-2" type="submit" disabled={!isEmpty(state.errors) || computed.isSubmitting}> {/* Add a button with type submit to call the submission callback if the button is a descended of the form element. */}
                    {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                    {computed.isSubmitting && <CircularProgress />}
                </Button>
            </div>
        </Stack>
    </form>
};
