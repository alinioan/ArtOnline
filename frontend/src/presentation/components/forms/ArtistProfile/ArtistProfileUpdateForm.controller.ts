import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { useCallback, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useQueryClient } from "@tanstack/react-query";
import { useAppSelector } from "@application/store";
import { useGetArtistProfileByUserId, useUpdateArtistProfile } from "@infrastructure/apis/api-management";
import {
    ArtistProfileUpdateFormController,
    ArtistProfileUpdateFormModel
} from "./ArtistProfileUpdateForm.types.ts";
import {useIntl} from "react-intl";
import {UserUpdateFormModel} from "@presentation/components/forms/User/updated/UserUpdateForm.types.ts";
import {isUndefined} from "lodash";

const getDefaultValues = (initialData?: UserUpdateFormModel) => {
    const defaultValues = {
        id: "",
        userId: "",
        bio: ""
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

const useInitArtistProfileUpdateForm = () => {
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        id: yup.string().nullable().optional(),
        userId: yup.string().nullable().optional(),
        bio: yup.string()
            .nullable()
            .optional()
            .default(defaultValues.bio)
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

export const useArtistProfileUpdateFormController = (onSubmit?: () => void): ArtistProfileUpdateFormController => {
    const { formatMessage } = useIntl();
    const { userId } = useAppSelector(x => x.profileReducer);
    const { data: artistProfile } = useGetArtistProfileByUserId(userId);
    const { mutateAsync: update, status } = useUpdateArtistProfile();
    const queryClient = useQueryClient();
    const { defaultValues, resolver } = useInitArtistProfileUpdateForm();

    const defaultValuesWithProfile = getDefaultValues(artistProfile?.response);

    const submit = useCallback((data: ArtistProfileUpdateFormModel) => {
        if (!data.id || !data.userId) {
            return Promise.reject(new Error("Artist profile data is required."));
        }

        return update({
            id: data.id,
            userId: data.userId,
            bio: data.bio ?? undefined
        }).then(() => {
            queryClient.invalidateQueries({ queryKey: ["getArtistProfileQuery"], type: "all" });
            if (onSubmit) {
                onSubmit();
            }
        });
    }, [onSubmit, queryClient, update]);

    const {
        register,
        handleSubmit,
        watch,
        reset,
        formState: { errors }
    } = useForm<ArtistProfileUpdateFormModel>({
        defaultValues: defaultValuesWithProfile,
        resolver
    });

    useEffect(() => {
        if (artistProfile?.response) {
            reset(getDefaultValues(artistProfile.response));
        }
    }, [artistProfile?.response, reset]);

    return {
        actions: {
            handleSubmit,
            submit,
            register,
            watch
        },
        computed: {
            isSubmitting: status === "pending"
        },
        state: {
            errors
        }
    }
};
