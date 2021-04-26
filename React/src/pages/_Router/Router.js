import "./pages.css";
import React from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";
import CRUDBook from "../Books/CRUDBook";
import LoginPage from "../LoginPage/LoginPage";
import CRUDCategory from "../Categories/CRUDCategory";
import UpdateBook from "../Books/UpdateBook";
import UpdateCategory from "../Categories/UpdateCategory";

export default function RouterApp() {
  return (
    <Router>
      <div>
        <nav>
          <ul className="horizontal">
            <li className="active">
              <Link to="/">HOME</Link>
            </li>
            <li>
              <Link to="/login">Login</Link>
            </li>
            <li>
              <Link to="/CRUDBook">Book</Link>
            </li>
            <li>
              <Link to="/CRUDCategory">Category</Link>
            </li>
          </ul>
        </nav>

        <Switch>
          <Route path="/login">
            <Login />
          </Route>
          <Route path="/CRUDBook">
            <CRUD_Book />
          </Route>
          <Route path="/CRUDCategory">
            <CRUD_Category />
          </Route>
          <Route path="/updateBook/:id">
            <UpdateBook />
          </Route>
          <Route path="/updateCategory/:id">
            <UpdateCategory />
          </Route>
          <Route path="/">
            <Home />
          </Route>
        </Switch>
      </div>
    </Router>
  );
}

function Home() {
  return <h1>Home</h1>;
}
function Login() {
  return <LoginPage />;
}

function CRUD_Book() {
  return <CRUDBook />;
}

function CRUD_Category() {
  return <CRUDCategory />;
}
