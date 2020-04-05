import * as React from 'react';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export default class NavMenu extends React.PureComponent<{}> {
    public render() {
        return (
            <header>
                <Navbar className="border-bottom">
                    <Container>
                        <NavbarBrand tag={Link} to="/">
                            nib
                        </NavbarBrand>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
