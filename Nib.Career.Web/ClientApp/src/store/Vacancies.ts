import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface LocationsState {
    locations: Location[];
}

export interface ResponseLocations {
    items: Location[];
}

export interface Location {
    id: string;
    city: string;
    state: string;
}

export interface VacanciesState {
    isLoading: boolean;
    isDetails: boolean;
    location?: number;
    skip: number;
    take: number;
    vacancies: Vacancies[];
    count: number;
    locations: Location[];
    vacancy?: Vacancies;
    vacancyId: string;
}

export interface ResponseVacancies {
    items: Vacancies[];
    count: number;
}

export interface Vacancies {
    id: string;
    createdDate: string;
    title: string;
    description: string;
    location: string;
}

interface RequestVacanciesAction {
    type: 'REQUEST_VACANCIES';
    location: number;
}

interface ReceiveVacanciesAction {
    type: 'RECEIVE_VACANCIES';
    location: number;
    vacancies: Vacancies[];
    count: number;
    skip: number;
    take: number;
}

interface RequestLocationsAction {
    type: 'REQUEST_LOCATIONS';
}

interface ReceiveLocationsAction {
    type: 'RECEIVE_LOCATIONS';
    locations: Location[];
}

interface RequestVacancyDetailsAction {
    type: 'REQUEST_VACANCY_DETAILS';
    id: string;
}

interface ReceiveVacancyDetailsAction {
    type: 'RECEIVE_VACANCY_DETAILS';
    id: string;
    vacancy: Vacancies;
}


type KnownAction = RequestVacanciesAction | ReceiveVacanciesAction | 
                    RequestLocationsAction | ReceiveLocationsAction |
                    RequestVacancyDetailsAction | ReceiveVacancyDetailsAction;

export const actionCreators = {
    requestVacancies: (location: number = 0, skip: number = 0, take: number = 0): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const url = 'http://localhost:5120';
        const appState = getState();
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ 
                locationId: location, 
                skip: skip, 
                take: take
            })
        };

        if (appState && appState.vacancies) {

                if ((location !== appState.vacancies.location
                    || skip !== appState.vacancies.skip
                    || take !== appState.vacancies.take) 
                    && appState.vacancies.vacancies.length === 0
                    && appState.vacancies.count !== 0
                    ) {
                
                    fetch(`${url}/v1/jobs`, requestOptions)
                        .then(response => response.json() as Promise<ResponseVacancies>)
                        .then(data => {
                            dispatch({ type: 'RECEIVE_VACANCIES', location: location, vacancies: data.items, count: data.count, skip: skip, take: take });
                        });

                        dispatch({ type: 'REQUEST_VACANCIES', location: location });
                }
        } 
    },
    requestLocations: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const url = 'http://localhost:5120';
        const appState = getState();
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        };

        if (appState && appState.vacancies && appState.vacancies.locations.length === 0) {
            fetch(`${url}/v1/locations`, requestOptions)
                .then(response => response.json() as Promise<ResponseLocations>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_LOCATIONS', locations: data.items });
                });

            dispatch({ type: 'REQUEST_LOCATIONS' });
        }
    },
    requestVacancyDetails: (id: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const url = 'http://localhost:5120';
        const appState = getState();
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        };

        if (appState && appState.vacancies && appState.vacancies.vacancyId !== id) {
            fetch(`${url}/v1/jobs/${id}`, requestOptions)
                .then(response => response.json() as Promise<Vacancies>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_VACANCY_DETAILS', vacancy: data, id: id });
                });

            dispatch({ type: 'REQUEST_VACANCY_DETAILS', id: id });
        }
    }
};

const unloadedState: VacanciesState = { 
    vacancies: [], 
    isLoading: false, 
    isDetails: false, 
    take: 20, 
    skip: 0, 
    count: -1, 
    locations: [], 
    location: -1,
    vacancyId: ""
};

export const reducer: Reducer<VacanciesState> = (state: VacanciesState | undefined, incomingAction: Action): VacanciesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_VACANCIES':
            return {
                location: action.location,
                vacancies: state.vacancies,
                isLoading: true,
                isDetails: false,
                take: state.take,
                skip: state.skip,
                count: state.count,
                locations: state.locations,
                vacancy: state.vacancy,
                vacancyId: state.vacancyId
            };
        case 'RECEIVE_VACANCIES':
            if (action.location === state.location
                && action.skip === state.skip
                && action.take === state.take) {
                return {
                    location: action.location,
                    vacancies: action.vacancies,
                    isLoading: false,
                    isDetails: false,
                    take: action.take,
                    skip: action.skip,
                    count: action.count,
                    locations: state.locations,
                    vacancy: state.vacancy,
                    vacancyId: "00000-000-0000-0000-0000000"
                };
            }
            break;
        case 'REQUEST_LOCATIONS':
            return {
                location: state.location,
                vacancies: state.vacancies,
                isLoading: false,
                isDetails: false,
                take: state.take,
                skip: state.skip,
                count: state.count,
                locations: state.locations,
                vacancy: state.vacancy,
                vacancyId: state.vacancyId
            };
        case 'RECEIVE_LOCATIONS':
            if (action.locations && action.locations.length !== state.locations.length) {
                return {
                    location: state.location,
                    vacancies: state.vacancies,
                    isLoading: false,
                    isDetails: false,
                    take: state.take,
                    skip: state.skip,
                    count: state.count,
                    locations: action.locations,
                    vacancy: state.vacancy,
                    vacancyId: state.vacancyId
                };
            }
            break;
        case 'REQUEST_VACANCY_DETAILS':
            return {
                location: state.location,
                vacancies: state.vacancies,
                isLoading: false,
                isDetails: true,
                take: state.take,
                skip: state.skip,
                count: state.count,
                locations: state.locations,
                vacancy: state.vacancy,
                vacancyId: action.id
            };
        case 'RECEIVE_VACANCY_DETAILS':
            if (action.id === state.vacancyId 
                && action.vacancy) {
                return {
                    location: state.location,
                    vacancies: [],
                    isLoading: false,
                    isDetails: true,
                    take: state.take,
                    skip: state.skip,
                    count: 0,
                    locations: state.locations,
                    vacancy: action.vacancy,
                    vacancyId: action.id
                };
            }
            break;
    }

    return state;
};
