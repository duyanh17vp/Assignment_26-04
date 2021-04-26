import * as React from "react";
import axios from "axios";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";

export default function AdminPage() {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState(null);
  const {
    register,
    setValue,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>();
  const onSubmit = handleSubmit((data) => console.log(data));

  useEffect(() => {
    (async () => {
      axios
        .get("http://localhost:5000/products")
        .then((res) => res.data)
        .then((data) => {
          setProducts(data);
        })
        .catch((err) => setError(err));
    })();
  }, []);

  return (
    <div>
      <h1>Category:</h1>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">ID</th>
            <th scope="col">UserName</th>
            <th scope="col">FullName</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody></tbody>
      </table>
      {error && <p>Something went wrong!</p>}

      <h1>Books:</h1>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">ID</th>
            <th scope="col">Name</th>
            <th scope="col">Status</th>
            <th scope="col">Title</th>
            <th scope="col">Category</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody></tbody>
      </table>
      {error && <p>Something went wrong!</p>}

      <h1>Users:</h1>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">ID</th>
            <th scope="col">UserName</th>
            <th scope="col">FullName</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody></tbody>
      </table>
      {error && <p>Something went wrong!</p>}
    </div>
  );
}
