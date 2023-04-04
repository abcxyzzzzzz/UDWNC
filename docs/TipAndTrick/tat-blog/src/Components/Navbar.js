import React from "react";
import {Navbar as Nb,Nav}from 'react-bootstrap';
import{
    Link
} from 'react-router-dom';

const Navbar = () =>{
    return (
        <Nb collapseOnSelect expand='sm'  bg ='white' variant='light'
        className="border-bottom shadow">
            <div className="container-fluid">
                <Nb.Brand href="/" > Tips anh Tricks</Nb.Brand>
                <Nb.Toggle aria-controls="responsive-navbar-nav"></Nb.Toggle>
                <Nb.Collapse id="responsive-navbar-nav"className="d-sm-inline-flex justify-content-between">
                    <Nav.Item>
                        <Link to = '/' className="nav-link text-dark">
                            Trang chu
                        </Link>
                    </Nav.Item>
                    <Nav.Item>
                        <Link to = '/blog/about' className="nav-link text-dark">
                            Gioi thieu
                        </Link>
                    </Nav.Item>
                    <Nav.Item>
                        <Link to = '/blog/contact' className="nav-link text-dark">
                            Lien He
                        </Link>
                    </Nav.Item>
                    <Nav.Item>
                        <Link to = '/blog/rss' className="nav-link text-dark">
                            Rss Feed
                        </Link>
                    </Nav.Item>
                </Nb.Collapse>
            </div>
        </Nb>
    )
}

export default Navbar;