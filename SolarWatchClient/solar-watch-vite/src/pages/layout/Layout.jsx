import { Outlet, Link } from "react-router-dom";

import './Layout.css';

const Layout = () => (
    <>
        <header>
            <div className="layout">
                <nav>
                    <ul>
                        <li className="solar-layout">
                            <Link to="/solar-watch">SolarWatch</Link>
                        </li>
                        <li className="registration-layout">
                            <Link to="/registration">Registration</Link>
                        </li>
                        <li className="login-layout">
                            <Link to="/login">Login</Link>
                        </li>
                    </ul>
                </nav>
            </div>
        </header>
        <Outlet />
    </>
)

export default Layout;