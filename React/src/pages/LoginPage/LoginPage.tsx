import axios from "axios";
import { watch } from "node:fs";
import React, { useRef, useState } from "react";
import ReactDOM from "react-dom";
import { Resolver, useForm } from "react-hook-form";
import { useHistory } from "react-router";
import { IAuthenticateModel } from "./Service/AuthenticateModel";

type FormData = {
  userName: string;
  password: string;
};

export default function LoginPage() {
  const [state, setState] = useState([]);
  const [error, setError] = useState(null);
  const {
    register,
    setValue,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>();
  let history = useHistory();
  const onSubmit = handleSubmit(async (dataModel: IAuthenticateModel) => {
    try {
      const res = await axios.post("https://localhost:5001/api/user/login", {
        userName: dataModel.userName,
        password: dataModel.password,
      });
      console.log(res.data);
      alert("logged in!");
      history.push("/");
      alert("Wellcome " + res.data.userName + " , Role: " + res.data.role);
    } catch (err) {
      setError(err);
      alert("login failed!");
      alert(err);
    }
  });

  return (
    <form onSubmit={onSubmit}>
      <h1>Login Form</h1>
      <label>UserName</label>
      <input
        {...register("userName", { required: "Username is required!" })}
        placeholder="username"
      />
      {errors?.userName && <p>{errors.userName.message}</p>}

      <label htmlFor="password">Password</label>
      <input
        {...register("password", {
          required: "Password is required",
          minLength: {
            value: 6,
            message: "Password must be at least 6 characters",
          },
        })}
        type="password"
        placeholder="password"
      />
      {errors?.password && <p>{errors.password.message}</p>}

      <input type="submit" value="Login" />
    </form>
  );
}
