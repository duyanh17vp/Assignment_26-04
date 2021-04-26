import React, { Component } from "react";
import "./App.css";
import RouterApp from "./pages/_Router/Router";
import LoginPage from "./pages/LoginPage/LoginPage";

export default class App extends Component {
  state = { value: 3 };
  render() {
    return (
      <div className="wrapper">
        <RouterApp />
      </div>
    );
  }
}
