import { UserUpdateFormController, UserUpdateFormModel } from "./UserUpdateForm.types.ts";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useQueryClient } from "@tanstack/react-query";
import { useUpdateUser } from "@infrastructure/apis/api-management";
import { useCallback } from "react";
import { useAppSelector } from "@application/store";
import {toast} from "react-toastify";

/**
 * Use a function to return the default values of the form and the validation schema.
 * You can add other values as the default, for example when populating the form with data to update an entity in the backend.
 */
const getDefaultValues = (initialData?: UserUpdateFormModel) => {
    const defaultValues = {
        id: "",
        name: "",
        password: ""
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

/**
 * Create a hook to get the validation schema.
 */
const useInitUserUpdateForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        name: yup.string()
            .nullable()
            .optional()
            .default(defaultValues.name),
        password: yup.string()
            .nullable()
            .optional()
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useUserUpdateFormController = (initialData?: UserUpdateFormModel, onSubmit?: () => void): UserUpdateFormController => {
    const { formatMessage } = useIntl();
    const { defaultValues, resolver } = useInitUserUpdateForm();
    const { mutateAsync: update, status } = useUpdateUser();
    const queryClient = useQueryClient();
    const { userId: currentUserId } = useAppSelector(x => x.profileReducer);

    const defaultValuesWithInitialData = getDefaultValues(initialData);

    const submit = useCallback((data: UserUpdateFormModel) => { // Create a submit callback to send the form data to the backend.
        console.log("Submitting data: ", data);
        console.log("Submitting data with id: ", currentUserId);
        const payload: UserUpdateFormModel = {
            id: currentUserId ?? "",
            name: data.name ?? undefined,
            password: data.password ?? undefined
        };

        toast("Updated successfully.", {data});
        return update(payload).then(() => {
            if (onSubmit) {
                onSubmit();
            }
        });
    }, [update, queryClient, currentUserId]);

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        formState: { errors }
    } = useForm<UserUpdateFormModel>({ // Use the useForm hook to get callbacks and variables to work with the form.
        defaultValues: defaultValuesWithInitialData, // Initialize the form with the default values.
        resolver // Add the validation resolver.
    });

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            watch, // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
        },
        computed: {
            defaultValues: defaultValuesWithInitialData,
            isSubmitting: status === "pending" // Return if the form is still submitting or nit.
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}
