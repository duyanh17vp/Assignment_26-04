import axios from "axios";
import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { IBook, IBookWithId } from "./Service/Book";

export default function CRUDBook() {
  const [books, setBooks] = useState([]);
  const [error, setError] = useState(null);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  useEffect(() => {
    (async () => {
      axios
        .get("https://localhost:5001/api/book")
        .then((res) => res.data)
        .then((data) => {
          setBooks(data);
        })
        .catch((err) => setError(err));
    })();
  }, []);

  const onSubmit = handleSubmit(async (dataBook: IBook) => {
    try {
      await axios.post("https://localhost:5001/api/book", {
        name: dataBook.name,
        title: dataBook.title,
        categoryId: dataBook.categoryId,
      });
      const res = await axios.get("https://localhost:5001/api/book");
      const data = res.data;
      setBooks(data);
    } catch (err) {
      setError(err);
    }
  });

  let OnDelete = async (id: number) => {
    try {
      await axios
        .delete(`https://localhost:5001/api/book/${id}`)
        .then((response) => {
          if (!(response.status == 204)) {
            alert("Delete failed!");
          } else {
            alert("Delete success!");
          }
        });
      const res = await axios.get("https://localhost:5001/api/book");
      const data = res.data;
      setBooks(data);
    } catch (err) {
      setError(err);
      alert("Delete failed!");
      alert(err);
    }
  };

  return (
    <div className="row">
      <div className="col-4">
        <form onSubmit={onSubmit}>
          <h1>Create Book</h1>
          <label>Book Name</label>
          <input {...register("name")} placeholder=" Name of Book" />
          <label>Title</label>
          <input {...register("title")} placeholder=" Title" />
          <label>CategoryId</label>
          <input {...register("categoryId")} placeholder=" CategoryId" />
          <input type="submit" value="Add" />
        </form>
      </div>
      <div className="col-8">
        <h1>ListBook</h1>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Name</th>
              <th scope="col">Title</th>
              <th scope="col">CategoryId</th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tbody>
            {books &&
              books.length > 0 &&
              books.map((book: IBookWithId) => (
                <tr key={book.id}>
                  <th scope="row">{book.id}</th>
                  <td>{book.name}</td>
                  <td>{book.title}</td>
                  <td>{book.categoryId}</td>
                  <td>
                    <article key={book.id}>
                      {/* <a href={`/detailsBook/${book.id}`}> Details </a> */}
                      <a href={`/updateBook/${book.id}`}> Update </a>
                      <a
                        href="#"
                        onClick={() => {
                          OnDelete(book.id);
                        }}
                      >
                        {" "}
                        Delete{" "}
                      </a>
                    </article>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
        {error && <p>Something went wrong!</p>}
      </div>
    </div>
  );
}
