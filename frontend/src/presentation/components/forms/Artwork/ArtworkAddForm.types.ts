import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";

export type ArtworkAddFormModel = {
    title: string;
    description?: string;
    file: File
};

export type ArtworkAddFormState = {
    errors: FieldErrorsImpl<DeepRequired<ArtworkAddFormModel>>;
};

export type ArtworkAddFormActions = {
    register: UseFormRegister<ArtworkAddFormModel>;
    watch: UseFormWatch<ArtworkAddFormModel>;
    handleSubmit: UseFormHandleSubmit<ArtworkAddFormModel>;
    submit: (body: ArtworkAddFormModel) => void;
    setFile: (file: File) => void;
};
export type ArtworkAddFormComputed = {
    isSubmitting: boolean
};

export type ArtworkAddFormController = FormController<ArtworkAddFormState, ArtworkAddFormActions, ArtworkAddFormComputed>;