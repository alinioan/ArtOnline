import {LoginRecord, RegisterRecord, UserAddRecord} from "../client/models";
import {AuthorizationApi, UserApi} from "../client/apis";
import {useMutation} from "@tanstack/react-query";

/**
 * Use constants to identify mutations and queries.
 */
const loginMutationKey = "loginMutation";
const registerMutationKey = "registerMutation";

/**
 * Returns the object with the callbacks that can be used for the React Query API, in this case just to log in the user.
 */
export const useLogin = () => {
    return useMutation({ // Return the mutation object.
        mutationKey: [loginMutationKey], // Add the key to identify the mutation.
        mutationFn: (loginRecord: LoginRecord) => new AuthorizationApi().apiAuthorizationLoginPost({loginRecord}) // Add the mutation callback by using the generated client code and adapt it.
    })
}

export const useRegister = () => {
    return useMutation({
        mutationKey: [registerMutationKey],
        mutationFn: (registerRecord: RegisterRecord) => new AuthorizationApi().apiAuthorizationRegisterPost({registerRecord})
    }
    );
}