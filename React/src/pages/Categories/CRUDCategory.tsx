import axios from "axios";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { ICategory } from "./Service/Category";

export default function CRUDCategory() {
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState(null);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  useEffect(() => {
    (async () => {
      axios
        .get("https://localhost:5001/api/category")
        .then((res) => res.data)
        .then((data) => {
          setCategories(data);
        })
        .catch((err) => setError(err));
    })();
  }, []);

  const onSubmit = handleSubmit(async (dataCate: ICategory) => {
    try {
      await axios.post("https://localhost:5001/api/category", {
        name: dataCate.name,
      });
      const res = await axios.get("https://localhost:5001/api/category");
      const data = res.data;
      setCategories(data);
    } catch (err) {
      setError(err);
    }
  });

  return (
    <div className="row">
      <div className="col-4">
        <form onSubmit={onSubmit}>
          <h1>Create Category</h1>
          <label>Category Name</label>
          <input {...register("name")} placeholder=" Name of Category" />
          <input type="submit" value="Add" />
        </form>
      </div>
      <div className="col-8">
        <div>
          <h1>List Category</h1>
          <table className="table">
            <thead>
              <tr>
                <th scope="col">ID</th>
                <th scope="col">Name</th>
                <th scope="col"></th>
              </tr>
            </thead>
            <tbody>
              {categories &&
                categories.length > 0 &&
                categories.map((category: ICategory) => (
                  <tr key={category.id}>
                    <th scope="row">{category.id}</th>
                    <td>{category.name}</td>
                    <td>
                      <article key={category.id}>
                        <a href={`/updateCategory/${category.id}`}> Update </a>
                        <a href={`/deleteCategory/${category.id}`}> Delete </a>
                      </article>
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
          {error && <p>Something went wrong!</p>}
        </div>
      </div>
    </div>
  );
}
