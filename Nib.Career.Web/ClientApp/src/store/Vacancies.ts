import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import { Vacancy } from './shared/model/Vacancy';
import { Location } from './shared/model/Location';

export interface LocationsState {
    locations: Location[];
}

export interface ResponseLocations {
    items: Location[];
}

export interface VacanciesState {
    isLoading: boolean;
    location?: number;
    skip: number;
    take: number;
    vacancies: Vacancy[];
    count: number;
    locations: Location[];
}

export interface ResponseVacancies {
    items: Vacancy[];
    count: number;
}

interface RequestVacanciesAction {
    type: 'REQUEST_VACANCIES';
    location: number;
    skip: number;
    take: number;
}

interface ReceiveVacanciesAction {
    type: 'RECEIVE_VACANCIES';
    location: number;
    vacancies: Vacancy[];
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


type KnownAction = RequestVacanciesAction | ReceiveVacanciesAction | 
                    RequestLocationsAction | ReceiveLocationsAction;

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
                    && !appState.vacancies.isLoading
                    ) {
                
                    fetch(`${url}/v1/jobs`, requestOptions)
                        .then(response => response.json() as Promise<ResponseVacancies>)
                        .then(data => {
                            dispatch({ type: 'RECEIVE_VACANCIES', location: location, vacancies: data.items, count: data.count, skip: skip, take: take });
                        });

                        dispatch({ type: 'REQUEST_VACANCIES', location: location, skip: skip, take: take });
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

        if (appState && appState.vacancies && appState.vacancies.locations.length === 0 && !appState.vacancies.isLoading) {
            fetch(`${url}/v1/locations`, requestOptions)
                .then(response => response.json() as Promise<ResponseLocations>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_LOCATIONS', locations: data.items });
                });
            dispatch({ type: 'REQUEST_LOCATIONS' });
        }

    }
};

const unloadedState: VacanciesState = { 
    vacancies: [], 
    isLoading: false,
    take: 20, 
    skip: 0, 
    count: -1, 
    locations: []
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
                take: action.take,
                skip: action.skip,
                count: state.count,
                locations: state.locations,
            };
        case 'RECEIVE_VACANCIES':
            if (action.location === state.location
                && action.skip === state.skip
                && action.take === state.take) {
                return {
                    location: action.location,
                    vacancies: action.vacancies,
                    isLoading: false,
                    take: action.take,
                    skip: action.skip,
                    count: action.count,
                    locations: state.locations
                };
            }
            break;
        case 'REQUEST_LOCATIONS':
            return {
                location: state.location,
                vacancies: state.vacancies,
                isLoading: true,
                take: state.take,
                skip: state.skip,
                count: state.count,
                locations: state.locations
            };
        case 'RECEIVE_LOCATIONS':
            if (action.locations) {
                return {
                    location: state.location,
                    vacancies: state.vacancies,
                    isLoading: false,
                    take: state.take,
                    skip: state.skip,
                    count: state.count,
                    locations: action.locations
                };
            }
            break;
    }

    return state;
};
