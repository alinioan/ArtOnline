import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { useQueryClient } from "@tanstack/react-query";
import { useAddArtwork, useGetArtistProfileByUserId } from "@infrastructure/apis/api-management";
import { useCallback } from "react";
import { useAppSelector } from "@application/store";
import {
    ArtworkAddFormController,
    ArtworkAddFormModel
} from "@presentation/components/forms/Artwork/ArtworkAddForm.types.ts";

/**
 * Create a hook to get the validation schema.
 */
const useInitArtworkAddForm = () => {
    const { formatMessage } = useIntl();

    const schema = yup.object().shape({
        file: yup.mixed<File>() // For files the schema used should be mixed.
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.file",
                    }),
                }))
    });

    const resolver = yupResolver(schema);

    return { defaultValues: {}, resolver };
}

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useArtworkAddFormController = (onSubmit?: () => void): ArtworkAddFormController => {
    const { defaultValues, resolver } = useInitArtworkAddForm();
    const { userId } = useAppSelector(x => x.profileReducer);
    const { data: artistProfile } = useGetArtistProfileByUserId(userId);
    const { mutateAsync: add, status } = useAddArtwork();
    const queryClient = useQueryClient();
    const submit = useCallback((data: ArtworkAddFormModel) => {
        const artistProfileId = artistProfile?.response?.id;

        if (!artistProfileId) {
            return Promise.reject(new Error("Artist profile id is required to add artwork."));
        }

        return add({
            title: data.title,
            description: data.description,
            imageFile: data.file,
            artistProfileId,
        }).then(() => {
            queryClient.invalidateQueries({ queryKey: ["getFeedQuery"] });
            if (onSubmit) {
                onSubmit();
            }
        });
    }, [add, artistProfile?.response?.id, onSubmit, queryClient]);

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        formState: { errors }
    } = useForm<ArtworkAddFormModel>({ // Use the useForm hook to get callbacks and variables to work with the form.
        defaultValues, // Initialize the form with the default values.
        resolver // Add the validation resolver.
    });

    const setFile = useCallback((file: File) => { // The file will be added via a button so create a callback to set the file value.
        setValue("file", file, {
            shouldValidate: true
        });
    }, [setValue]);

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            watch,  // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
            setFile
        },
        computed: {
            isSubmitting: status === "pending" // Return if the form is still submitting or nit.
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}