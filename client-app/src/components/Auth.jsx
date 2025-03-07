import { useState } from "react";
import axios from "axios";

export default function Auth() {
    const [form, setForm] = useState({
        username: "",
        email: "",
        password: "",
        isLogin: true, // Toggle between login & signup
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const url = form.isLogin ? "http://localhost:5000/api/auth/login" : "http://localhost:5000/api/auth/register";

        try {
            const response = await axios.post(url, {
                username: form.isLogin ? undefined : form.username, // Send username only if signing up
                email: form.email,
                password: form.password,
            });

            alert(form.isLogin ? "Login successful!" : "Sign-up successful!");
            localStorage.setItem("token", response.data.token);
            console.log(response.data);
        } catch (error) {
            console.error("Error:", error);
            alert(error.response?.data?.message || "Something went wrong!");
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="w-full max-w-md bg-white shadow-lg rounded-2xl p-6">
                <h2 className="text-2xl font-bold text-center mb-6">{form.isLogin ? "Log in" : "Sign Up"}</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    {!form.isLogin && (
                        <div>
                            <input
                                type="text"
                                name="username"
                                className="w-full p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                placeholder="Username"
                                value={form.username}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    )}
                    <div>
                        <input
                            type="email"
                            name="email"
                            className="w-full p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
                            placeholder="Email"
                            value={form.email}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div>
                        <input
                            type="password"
                            name="password"
                            className="w-full p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
                            placeholder="Password"
                            value={form.password}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div>
                        <button type="submit" className="w-full bg-indigo-600 text-white py-3 rounded-lg hover:bg-indigo-700 transition">
                            {form.isLogin ? "Login" : "Sign Up"}
                        </button>
                    </div>
                </form>
                <p className="text-center mt-4">
                    {form.isLogin ? "Don't have an account?" : "Already have an account?"}{" "}
                    <span
                        className="text-indigo-600 cursor-pointer hover:underline"
                        onClick={() => setForm({ ...form, isLogin: !form.isLogin })}
                    >
                        {form.isLogin ? "Sign Up" : "Login"}
                    </span>
                </p>
            </div>
        </div>
    );
}
