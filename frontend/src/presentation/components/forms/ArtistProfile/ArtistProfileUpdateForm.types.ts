import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";

export type ArtistProfileUpdateFormModel = {
    id: string;
    userId: string;
    bio?: string;
};

export type ArtistProfileUpdateFormState = {
    errors: FieldErrorsImpl<DeepRequired<ArtistProfileUpdateFormModel>>;
};

export type ArtistProfileUpdateFormActions = {
    register: UseFormRegister<ArtistProfileUpdateFormModel>;
    watch: UseFormWatch<ArtistProfileUpdateFormModel>;
    handleSubmit: UseFormHandleSubmit<ArtistProfileUpdateFormModel>;
    submit: (body: ArtistProfileUpdateFormModel) => Promise<void>;
};

export type ArtistProfileUpdateFormComputed = {
    isSubmitting: boolean;
};

export type ArtistProfileUpdateFormController = FormController<ArtistProfileUpdateFormState, ArtistProfileUpdateFormActions, ArtistProfileUpdateFormComputed>;
