import axios from "axios";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useHistory, useParams } from "react-router";
import { IBook } from "./Service/Book";

export default function UpdateBook() {
  const [books, setBooks] = useState([]);
  const [error, setError] = useState(null);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  let { id } = useParams<any>();
  let history = useHistory();

  const onSubmit = handleSubmit(async (data: IBook) => {
    try {
      await axios.put(`https://localhost:5001/api/book/${id}`, {
        id: id,
        name: data.name,
        title: data.title,
        categoryId: data.categoryId,
      });
      history.push("/CRUDBook");
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
        <h1>Update Book</h1>
        <label>Book Name</label>
        <input {...register("name")} placeholder=" Name of Book" />
        <label>Title</label>
        <input {...register("title")} placeholder=" Title" />
        <label>CategoryId</label>
        <input {...register("categoryId")} placeholder=" CategoryId" />
        <input type="submit" value="Add" />
      </form>
    </div>
  );
}
