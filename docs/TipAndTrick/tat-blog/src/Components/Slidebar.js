import React from 'react';
import SearchForm from './SearchForm';
import CategoriesWidget from './CategoriesWidget';
const Sidebar =() => {
    return (
        <div className='pt-4 ps-2'> 
        <SearchForm/>
        <CategoriesWidget />
            <h1>
            Tìm kiểm bài viết 
            </h1>
            <h1>
            các chủ để
            </h1>
            <h1>
            Bài viết nổi bật 
            </h1>
            <h1>
            Đằng ký nhận tỉn mới 
            </h1>
            <h1>
        Tag cloud </h1>
        </div>
    )
}

export default Sidebar;