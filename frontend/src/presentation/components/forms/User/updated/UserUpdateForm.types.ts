import { FormController } from "../../FormController.ts";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";

export type UserUpdateFormModel = {
    id: string;
    name?: string | null;
    password?: string | null;
};

export type UserUpdateFormState = {
    errors: FieldErrorsImpl<DeepRequired<UserUpdateFormModel>>;
};

export type UserUpdateFormActions = {
    register: UseFormRegister<UserUpdateFormModel>;
    watch: UseFormWatch<UserUpdateFormModel>;
    handleSubmit: UseFormHandleSubmit<UserUpdateFormModel>;
    submit: (body: UserUpdateFormModel) => void;
};

export type UserUpdateFormComputed = {
    defaultValues: UserUpdateFormModel,
    isSubmitting: boolean
};

export type UserUpdateFormController = FormController<UserUpdateFormState, UserUpdateFormActions, UserUpdateFormComputed>;
