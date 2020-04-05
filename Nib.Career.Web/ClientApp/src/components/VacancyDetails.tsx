import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as VacancyDetailsStore from '../store/VacanciesDetails';
import { Container } from 'reactstrap';
import Moment from 'react-moment';

type Props = VacancyDetailsStore.VacancyState
  & typeof VacancyDetailsStore.actionCreators
  & RouteComponentProps<{ id: string }>;

class VacanciesDetails extends React.PureComponent<Props> {
  
  public componentDidMount() {
    this.ensureDataFetched();
  }

  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public render() {
    return (
        <React.Fragment>
          <div className={`outer-container vacancies-container overlay`}>
            <Container>
              <div className="vacancy-header">
                <h1 id="listTable">Our Vacancies</h1>
              </div>
              <hr />
              {this.renderVacancy()}
            </Container>
          </div>
        </React.Fragment>
    );
  }

  private ensureDataFetched() {
    const id = this.props.match.params.id;
    this.props.requestVacancyDetails(id);
  }


  private renderVacancy() {
    const vacancy = this.props.vacancy
    if (vacancy) {
      return (
        <div key={vacancy.id} className="vacancy-card">
          <div className="vacancy-card-content">
            <div className="vacancy-card-content-row">
              <div className="vacancy-card-content-title"><h3>{vacancy.title}</h3></div>
              <div className="vacancy-card-content-location hide-content-sm"><p>{vacancy.location}</p></div>
            </div>
            <div className="vacancy-card-content-row hide-content-md hide-content-lg">
              <div className="vacancy-card-content-location"><p>{vacancy.location}</p></div>
              <div className="vacancy-card-content-date"><Moment format="DD, MMMM">{vacancy.createdDate}</Moment></div>
            </div>
            <div className="vacancy-card-content-description">{vacancy.description}</div>
          </div>
          <div className="vacancy-card-date hide-content-sm">
            <div className="vacancy-card-date-day">
              <Moment format="DD">{vacancy.createdDate}</Moment>
            </div>
            <div className="vacancy-card-date-month">
              <Moment format="MMMM">{vacancy.createdDate}</Moment>
            </div>
          </div>
        </div>
      );
    }

    return (
      <div>No job found.</div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.vacancyDetails,
  VacancyDetailsStore.actionCreators
)(VacanciesDetails as any);
