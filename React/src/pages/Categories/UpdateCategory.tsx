import axios from "axios";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useHistory, useParams } from "react-router";
import { ICategory } from "./Service/Category";

export default function UpdateCategory() {
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState(null);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  let { id } = useParams<any>();
  let history = useHistory();

  const onSubmit = handleSubmit(async (dataCate: ICategory) => {
    try {
      await axios.put(`https://localhost:5001/api/category/${id}`, {
        id: id,
        name: dataCate.name,
      });
      history.push("/CRUDCategory");
      alert("Update success!");
    } catch (err) {
      setError(err);
      alert("Update failed!");
      alert(err);
    }
  });

  return (
    <div>
      <form onSubmit={onSubmit}>
        <h1>Create Category</h1>
        <label>Category Name</label>
        <input {...register("name")} placeholder=" Name of Category" />
        <input type="submit" value="Add" />
      </form>
    </div>
  );
}
