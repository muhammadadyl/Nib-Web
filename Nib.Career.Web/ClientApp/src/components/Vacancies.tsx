import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as VacanciesStore from '../store/Vacancies';
import { Container } from 'reactstrap';
import Moment from 'react-moment';

type Props = VacanciesStore.VacanciesState
  & typeof VacanciesStore.actionCreators
  & RouteComponentProps<{ location: string, page: string }>;

class VacanciesData extends React.PureComponent<Props> {

  /**
   * constructor to register variables to event
   */
  constructor(props: any) {
    super(props);
    
    this.handleChangeLocation = this.handleChangeLocation.bind(this);
  }

  private pageSize = 20;
  
  public componentDidMount() {
    this.ensureDataFetched();
  }

  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public handleChangeLocation(event: any) {
    this.props.history.push(`/location/${event.target.value}`);
  }

  public render() {
    return (
        <React.Fragment>
          <div className={`outer-container vacancies-container ${ this.props.isDetails? "hide-content" : "" }`}>
            <Container>
              <div className="vacancy-header">
                <h1 id="listTable">Our Vacancies</h1>
                <div className="vacancy-location-select">
                  {this.renderLocationsSelect()}
                </div>
              </div>
              <hr />
              {this.renderVacanciesList()}
            </Container>
          </div>
        </React.Fragment>
    );
  }

  private ensureDataFetched() {
    const location = parseInt(this.props.match.params.location, 10) || 0;
    const page = parseInt(this.props.match.params.page, 10) || 1;
    this.props.requestVacancies(location, (page - 1) * this.pageSize, this.pageSize);
    this.props.requestLocations();
  }

  private renderLocationsSelect() {
    return(
      <select
      className="vacancy-location-select"
      value={this.props.location}
      onChange={this.handleChangeLocation}>
          <option key="0" value="0">-----All-----</option>
          {this.props.locations.map((location) =>
            <option key={location.id} value={location.id}>
              {location.city}, {location.state}
            </option>
          )}
      </select>
    );
  }

  private renderVacanciesList() {
    return (
      <div className="vacancy-list">
        {this.props.vacancies.map((vacancy: VacanciesStore.Vacancies) =>
        <Link key={vacancy.id} className="vacancy-link" to={`/vacancy/details/${vacancy.id}`}>
          <div className="vacancy-card">
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
        </Link>
        )}
        {this.renderPagination()}
      </div>
    );
  }

  private renderPagination() {
    const maxpage = Math.ceil(this.props.count / this.pageSize);
    const page = parseInt(this.props.match.params.page, 10) || 1;
    const location = parseInt(this.props.match.params.location, 10) || 0;
    const prevStartIndex = page - 1;
    const nextStartIndex = page + 1;

    return (
      <div className="vacancy-pagination">
        <Link className={`btn btn-outline-primary ${ page === 1 ? "disable-link" : "" }`} to={`/location/${location}/${prevStartIndex}`}>Previous</Link>
        {this.props.isLoading && <span>Loading...</span>}
        <Link className={`btn btn-outline-primary ${ page === maxpage ? "disable-link" : "" }`} to={`/location/${location}/${nextStartIndex}`}>Next</Link>
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.vacancies, // Selects which state properties are merged into the component's props
  VacanciesStore.actionCreators // Selects which action creators are merged into the component's props
)(VacanciesData as any);
