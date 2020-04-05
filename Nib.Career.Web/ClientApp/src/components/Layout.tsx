import * as React from 'react';
import NavMenu from './NavMenu';
import { Container } from 'reactstrap';

export default (props: { children?: React.ReactNode }) => (
    <React.Fragment>
        <NavMenu/>
        <div className="outer-container">
            <div className="banner">
            <div className="banner-content">
                <h1>Love your careers.</h1>
                <h1>It's worth it.</h1>
            </div>
            </div>
            <Container>
                <div className="icon-container">
                    <div className="icon">
                    <img src="/content/clock.svg" alt="flexible work" />  
                    </div>
                    <p>
                    Flexible work hours to suit your lifestyle
                    </p>
                </div>
                <div className="icon-container">
                    <div className="icon">
                    <img src="/content/bike.svg" alt="a new bike" />
                    </div>
                    <p>
                    A new bike to get around town!
                    </p>
                </div>
                <div className="icon-container">
                    <div className="icon">
                    <img src="/content/umbrella.svg" alt="discounted insurance" />
                    </div>
                    <p>
                    Discounted health insurance
                    </p>
                </div>
                <div className="icon-container">
                    <div className="icon">
                    <img src="/content/pins.svg" alt="contemporary offices" />
                    </div>
                    <p>
                    Contemporary offices in a great location
                    </p>
                </div>
            </Container>
            {props.children}
        </div>
    </React.Fragment>
);
