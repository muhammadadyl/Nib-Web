import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Vacancies from './components/Vacancies';
import VacancyDetails from './components/VacancyDetails';

import './custom.css'

export default () => (
    <Layout>
        <Route path='/' component={Vacancies} />
        <Route path='/location/:location?/:page?' component={Vacancies} />
        <Route path='/vacancy/details/:id' component={VacancyDetails} />
    </Layout>
);
