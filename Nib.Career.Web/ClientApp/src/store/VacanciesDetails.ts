import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import { Vacancy } from './shared/model/Vacancy';

export interface VacancyState {
    vacancy?: Vacancy;
    vacancyId: string;
}

interface RequestVacancyDetailsAction {
    type: 'REQUEST_VACANCY_DETAILS';
    id: string;
}

interface ReceiveVacancyDetailsAction {
    type: 'RECEIVE_VACANCY_DETAILS';
    id: string;
    vacancy: Vacancy;
}


type KnownAction = RequestVacancyDetailsAction | ReceiveVacancyDetailsAction;

export const actionCreators = {
    requestVacancyDetails: (id: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const url = 'http://localhost:5120';
        const appState = getState();
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        };

        if (appState && appState.vacancyDetails && appState.vacancyDetails.vacancyId !== id) {
            fetch(`${url}/v1/jobs/${id}`, requestOptions)
                .then(response => response.json() as Promise<Vacancy>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_VACANCY_DETAILS', vacancy: data, id: id });
                });

            dispatch({ type: 'REQUEST_VACANCY_DETAILS', id: id });
        }
    }
};

const unloadedState: VacancyState = {
    vacancyId: ""
};

export const reducer: Reducer<VacancyState> = (state: VacancyState | undefined, incomingAction: Action): VacancyState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_VACANCY_DETAILS':
            return {
                vacancy: state.vacancy,
                vacancyId: action.id
            };
        case 'RECEIVE_VACANCY_DETAILS':
            if (action.id === state.vacancyId 
                && action.vacancy) {
                return {
                    vacancy: action.vacancy,
                    vacancyId: action.id
                };
            }
            break;
    }

    return state;
};
