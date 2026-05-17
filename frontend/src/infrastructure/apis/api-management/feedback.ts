import {useAppSelector} from "@application/store";
import {Configuration, FeedbackFormApi, FeedbackSubmitRecord} from "@infrastructure/apis/client";
import {useMutation, useQueryClient} from "@tanstack/react-query";

const submitFeedbackMutationKey = "submitFeedbackMutation";

const getFactory = (token: string | null) => new FeedbackFormApi(new Configuration({accessToken: token ?? ""}));

export const useSubmitFeedback = () => {
    const {token} = useAppSelector(x => x.profileReducer);
    const queryClient = useQueryClient();

    return useMutation({
        mutationKey: [submitFeedbackMutationKey, token],
        mutationFn: async (feedback: FeedbackSubmitRecord) => {
            const result = await getFactory(token).apiFeedbackFormSubmitPost({feedbackSubmitRecord: feedback});
            await queryClient.invalidateQueries({queryKey: [submitFeedbackMutationKey], type: "all"});
            return result;
        }
    })
}
