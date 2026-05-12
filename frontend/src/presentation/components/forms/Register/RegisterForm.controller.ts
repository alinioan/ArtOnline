import { RegisterFormController, RegisterFormModel } from "./RegisterForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { useCallback } from "react";
import { useRegister } from "@infrastructure/apis/api-management";
import { useAppRouter } from "@infrastructure/hooks/useAppRouter";
import { toast } from "react-toastify";
import { UserRoleEnum } from "@infrastructure/apis/client";
import { AppRoute } from "routes";

const getDefaultValues = () => ({
    name: "",
    email: "",
    password: "",
    confirmPassword: ""
});

const useInitRegisterForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        name: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({ id: "globals.name" })
                }))
            .default(defaultValues.name),
        email: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({ id: "globals.email" })
                }))
            .email()
            .default(defaultValues.email),
        password: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({ id: "globals.password" })
                }))
            .default(defaultValues.password),
        confirmPassword: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({ id: "globals.confirmPassword" })
                }))
            .oneOf([yup.ref("password")], formatMessage({ id: "globals.validations.passwordMismatch" }))
            .default(defaultValues.confirmPassword),
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
};

export const useRegisterFormController = (): RegisterFormController => {
    const { formatMessage } = useIntl();
    const { defaultValues, resolver } = useInitRegisterForm();
    const { navigate } = useAppRouter();
    const { mutateAsync: register, status } = useRegister();

    const submit = useCallback((data: RegisterFormModel) =>
        register({ ...data })
            .then(() => {
                toast(formatMessage({ id: "notifications.messages.registrationSuccess" }));
                navigate(AppRoute.Login);
            }), [register, navigate, formatMessage]);

    const {
        register: registerField,
        handleSubmit,
        formState: { errors }
    } = useForm<RegisterFormModel>({
        defaultValues,
        resolver
    });

    return {
        actions: {
            handleSubmit,
            submit,
            register: registerField
        },
        computed: {
            defaultValues,
            isSubmitting: status === "pending"
        },
        state: {
            errors
        }
    };
};
