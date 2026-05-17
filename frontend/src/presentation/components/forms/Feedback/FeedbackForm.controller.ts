import { FeedbackFormController, FeedbackFormModel } from "./FeedbackForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useCallback } from "react";
import { SelectChangeEvent } from "@mui/material";
import { ChangeEvent } from "react";
import { toast } from "react-toastify";
import { useSubmitFeedback } from "@infrastructure/apis/api-management/feedback";

const getDefaultValues = (initialData?: FeedbackFormModel) => {
    const defaultValues = {
        rating: "",
        category: "",
        message: "",
        subscribe: false
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

const useInitFeedbackForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        rating: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.rating",
                    }),
                }))
            .default(defaultValues.rating),
        category: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.category",
                    }),
                }))
            .default(defaultValues.category),
        message: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.message",
                    }),
                }))
            .default(defaultValues.message),
        subscribe: yup.boolean()
            .default(defaultValues.subscribe)
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

export const useFeedbackFormController = (onSubmit?: () => void): FeedbackFormController => {
    const { formatMessage } = useIntl();
    const { defaultValues, resolver } = useInitFeedbackForm();
    const { mutateAsync: submitFeedback, status } = useSubmitFeedback();

    const submit = useCallback((data: FeedbackFormModel) => { // Create a submit callback to send the form data to the backend.
        const payload = {
            options: [
                {
                    category: data.category,
                    satisfactionLevel: data.rating
                }
            ],
            message: data.message,
            contactReason: data.category ?? null,
            contactEmail: null
        };

        return submitFeedback(payload).then(() => {
            toast("Feedback sent!");
            if (onSubmit) {
                onSubmit();
            }
        });
    }, [formatMessage, onSubmit, submitFeedback]);

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        formState: { errors }
    } = useForm<FeedbackFormModel>({
        defaultValues,
        resolver
    });

    const selectCategory = useCallback((event: SelectChangeEvent<string>) => { // Select inputs are tricky and may need their on callbacks to set the values.
        setValue("category", event.target.value as string, {
            shouldValidate: true,
        });
    }, [setValue]);

    const selectRating = useCallback((event: ChangeEvent<HTMLInputElement>) => {
        setValue("rating", event.target.value as string, { shouldValidate: true });
    }, [setValue]);

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            watch, // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
            selectCategory,
            selectRating
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading" // Return if the form is still submitting or not.
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}
